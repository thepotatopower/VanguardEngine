-- 樹角獣 アンヴァール

function RegisterAbilities()
	-- on place or put
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTiming(a.PutOnGC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCondition("OnPlaceCondition")
	ability1.SetActivation("OnPlace")
	-- on end of battle
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnBattleEnds)
	ability2.SetLocation(l.BackRowRC)
	ability2.SetTrigger("OnBattleEndsTrigger")
	ability2.SetCondition("OnBattleEndsCondition")
	ability2.SetActivation("OnBattleEnds")
	ability2.SetProperty(p.NotMandatory)
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlaceCondition()
	return obj.IsSameZone() and obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dsd04_001")}) 
end

function OnPlace()
	if obj.IsSameZone() then
		obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.This}, cs.BonusPower, 5000, p.UntilEndOfTurn)
		obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.This}, cs.BonusShield, 5000, p.UntilEndOfTurn)
	end
end

function OnBattleEndsTrigger()
	return obj.IsAttackingUnit()
end

function OnBattleEndsCondition()
	return obj.IsSameZone()
end

function OnBattleEnds()
	if obj.IsSameZone() then
		obj.AddToHand({q.Location, l.PlayerUnits, q.Other, o.This})
	end
end