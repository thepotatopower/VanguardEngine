-- 悪逆非道？のモスガール メープル

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnVC)
	ability1.SetLocation(l.Drop)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCondition("OnPlaceCondition")
	ability1.SetActivation("OnPlace")
	ability1.SetProperty(p.NotMandatory)
	-- on battle ends
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnBattleEnds)
	ability2.SetLocation(l.RC)
	ability2.SetTrigger("OnBattleEndsTrigger")
	ability2.SetCost("OnBattleEndsCost")
	ability2.SetActivation("OnBattleEnds")
end

function OnPlaceTrigger()
	return obj.PlayerRidePhase() and obj.Exists({q.Location, l.Applicable, q.Grade, 3, q.Other, o.GradeOrHigher})
end

function OnPlaceCondition()
	return not obj.Exists({q.Location, l.PlayerUnits, q.NameContains, obj.LoadName("Maple")})
end

function OnPlace()
	obj.SuperiorCall({q.Other, o.ThisFieldID, q.Count, 1})
end

function OnBattleEndsTrigger()
	return obj.IsAttackingUnit()
end

function OnBattleEndsCost(check)
	if check then return obj.CanSB(1) end
	obj.Store(obj.SpecificSoulBlast({q.Location, l.Soul, q.Min, 1}))
end

function OnBattleEnds()
	local grade = obj.GetSumOfGrades({q.Location, l.Stored})
	obj.ChooseAddToHand({q.Location, l.Soul, q.Grade, grade, q.Count, 1})
end