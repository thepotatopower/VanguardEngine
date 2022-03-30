-- 小さな賢者 マロン

function RegisterAbilities()
	-- on ride
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRide)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("OnRide")
	-- cont
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.RC)
	ability2.SetActivation("Cont")
end

function Trigger()
	return obj.WasRodeUponByNameContains(obj.LoadName("Blaster")) and obj.WasRodeUponByGradeIs(2)
end

function OnRide()
	obj.LookAtTopOfDeck(7)
	obj.Store(obj.Search({q.Location, l.Looking, q.Grade, 2, q.NameContains, obj.LoadName("Blaster"), q.Count, 1, q.Min, 0}))
	if not obj.Exists({q.Location, l.Stored}) then
		obj.SuperiorCall({q.Location, l.Soul, q.Name, obj.GetNameFromCardID("dbt05_083"), q.Count, 1, q.Min, 0})
	end
end

function Cont()
	if obj.IsPlayerTurn() and obj.GetNumberOf({q.Location, l.PlayerUnits}) >= 3 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 2000, p.Continuous)
	end
end