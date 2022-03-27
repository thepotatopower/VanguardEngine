-- ヴェルリーナ・エルガー

function RegisterAbilities()
	-- overdress
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetOverDress("IsOverDressTarget")
	-- on battle ends
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnBattleEnds)
	ability2.SetLocation(l.Hand)
	ability2.SetTrigger("Trigger")
	ability2.SetCondition("Condition")
	ability2.SetCost("Cost")
	ability2.SetCanFullyResolve("CanFullyResolve")
	ability2.SetActivation("Activation")
	-- cont
	local ability3 = NewAbility(GetID())
	ability3.SetDescription(3)
	ability3.SetTiming(a.Cont)
	ability3.SetActivation("Cont")
end

function IsOverDressTarget(id)
	return obj.IsName(id, obj.GetNameFromCardID("dsd01_009"))
end

function Trigger()
	return obj.Exists({q.Location, l.PlayerUnits, q.Name, obj.GetNameFromCardID("dsd01_009"), q.Other, o.Attacking}) and
	obj.Exists({q.Location, l.EnemyVC, q.Other, o.Attacked})
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dsd01_001")})
end

function Cost(check)
	if check then return obj.CanSB(2) end
	obj.SoulBlast(2)
end

function CanFullyResolve()
	return obj.IsSameZone() and obj.Exists({q.Location, l.PlayerUnits, q.Name, obj.GetNameFromCardID("dsd01_009"), q.Other, o.Attacking})
end

function Activation()
	local circle = obj.GetCircle({q.Location, l.PlayerUnits, q.Name, obj.GetNameFromCardID("dsd01_009"), q.Other, o.Attacking})
	if circle >= 0 then
		obj.SuperiorCall({"Filter"}, {l.Hand}, 1, 1, "", {p.SuperiorOverDress, p.DoNotConvert}, {circle})
	end
	obj.CounterCharge(1)
end

function Filter(id)
	return obj.IsThisFieldID(id)
end

function Cont()
	if obj.IsOverDress() then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 10000, p.Continuous)
		obj.AddCardValue({q.Other, o.This}, cs.BonusShield, 10000, p.Continuous)
	end
end
