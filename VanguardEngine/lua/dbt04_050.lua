-- 誓約の天刃 フリエント

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Cont")
	-- on stand
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnStand)
	ability2.SetLocation(l.RC)
	ability2.SetSourceIsPlayer(true)
	ability2.SetTrigger("OnStandTrigger")
	ability2.SetCondition("OnStandCondition")
	ability2.SetActivation("OnStand")
end

function Cont()
	if obj.IsPlayerTurn() and obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt04_010")}) then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end

function OnStandTrigger()
	return obj.IsApplicable()
end

function OnStandCondition()
	return obj.Exists({q.Location, l.EnemyVC, q.Grade, 3, q.Other, o.GradeOrHigher}) and obj.IsSameZone()
end

function OnStand()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end