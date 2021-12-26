-- 焔の巫女 リノ

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
	ability2.SetLocation(l.VC)
	ability2.SetLocation(l.RC)
	ability2.SetActivation("Cont")
end

function OnRideTrigger()
	return obj.WasRodeUponBy(obj.GetNameFromCardID("dsd01_002"))
end

function OnRideCondition()
	return obj.CanSuperiorCall({q.Location, l.Deck, q.Name, obj.GetNameFromCardID("dsd01_009")})
end

function OnRide()
	obj.SuperiorCall({q.Location, l.Deck, q.Name, obj.GetNameFromCardID("dsd01_009"), q.Count, 1, q.Min, 0})
end

function Cont()
	if obj.IsAttackingUnit() then
		obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.This}, cs.BonusPower, 2000, p.Continuous)
	end
end