-- インロード・シューター

function RegisterAbilities()
	-- on ride
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRide)
	ability1.SetTrigger("OnRideTrigger")
	ability1.SetCondition("OnRideCondition")
	ability1.SetActivation("OnRide")
	-- cont
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.PlayerRC)
	ability2.SetActivation("Cont")
end

function OnRideTrigger()
	return obj.WasRodeUponBy(obj.GetNameFromCardID("dbt03_050")) 
end

function OnRideCondition()
	return obj.Exists({q.Location, l.Soul, q.Other, o.This})
end

function OnRide()
	obj.SuperiorCall({q.Location, l.Soul, q.Other, o.This})
end

function Cont()
	if obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt03_009"), q.Name, obj.GetNameFromCardID("dbt03_050")}) then
		obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusSkills, s.Intercept, p.Continuous)
		obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusPower, 2000, p.Continuous)
	end
end