-- 焔の巫女 レイユ

function RegisterAbilities()
	-- on ride
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRide)
	ability1.SetTrigger("OnRideTrigger")
	ability1.SetCost("OnRideCost")
	ability1.SetCanFullyResolve("OnRideCanFullyResolve")
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
	return obj.WasRodeUponBy(obj.GetNameFromCardID("dsd01_001"))
end

function OnRideCost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function OnRideCanFullyResolve()
	return obj.CanSearch({q.Location, l.Deck, q.Name, obj.GetNameFromCardID("dsd01_006"), q.Count, 1})
end

function OnRide()
	obj.Search({q.Location, l.Deck, q.Name, obj.GetNameFromCardID("dsd01_006"), q.Count, 1})
end

function Cont()
	if obj.IsAttackingUnit() then
		obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.This}, cs.BonusPower, 2000, p.Continuous)
	end
end