-- 華やぐ旋律 レクティナ

function RegisterAbilities()
	-- on ride
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRide)
	ability1.SetTriggerCondition("Trigger")
	ability1.SetActivation("OnRide")
	ability1.SetCost("Cost")
	-- cont
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.RC)
	ability2.SetActivation("Cont")
end

function Trigger()
	return obj.WasRodeUponByNameContains(obj.LoadName("Lianorn"))
end

function Cost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function CanFullyResolve()
	return obj.CanSuperiorCall({q.Location, l.Drop})
end

function OnRide()
	obj.SuperiorCall({q.Location, l.Drop, q.Count, 1})
end

function Cont()
	if obj.GetNumberOf({q.Location, l.BackRowRC}) >= 3 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusSkills, s.Boost, p.Continuous)
	end
end