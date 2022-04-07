-- アセンダンス・アサルト

function RegisterAbilities()
	-- on ride
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRide)
	ability1.SetTrigger("OnRideTrigger")
	ability1.SetCost("OnRideCost")
	ability1.SetCanFullyResolve("OnRideCanFullyResolve")
	ability1.SetActivation("OnRide")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.PlayerRC)
	ability2.SetProperty(p.OncePerTurn)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCondition("OnAttackCondition")
	ability2.SetCanFullyResolve("OnAttackCanFullyResolve")
	ability2.SetCost("OnAttackCost")
	ability2.SetActivation("OnAttack")
end

function OnRideTrigger()
	return obj.IsApplicable() 
end

function OnRideCost(check)
	local specificSB = {q.Location, l.Soul, q.Other, o.NotThis, q.Count, 1}
	local reveal = {q.Location, l.Hand, q.Name, obj.GetNameFromCardID("dbt03_009"), q.Name, obj.GetNameFromCardID("dbt03_113"), q.Count, 1}
	if check then return obj.CanSpecificSB(specificSB) or obj.CanReveal(reveal) end
	if obj.CanSpecificSB(specificSB) and obj.CanReveal(reveal) and obj.YesNo(obj.GetDescription(3)) then
		obj.ChooseReveal(reveal)
	elseif not obj.CanSpecificSB(specificSB) then
		obj.ChooseReveal(reveal)
	else
		obj.SpecificSoulBlast(specificSB)
	end
end

function OnRideCanFullyResolve()
	return obj.Exists({q.Location, l.Soul, q.Other, o.This})
end

function OnRide()
	obj.SuperiorCall({q.Location, l.Soul, q.Other, o.This})
end

function OnAttackTrigger()
	return obj.Exists({q.Location, l.PlayerRC, q.Other, o.Attacking, q.Other, o.NotThis}) 
end

function OnAttackCondition()
	return obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt03_009")})
end

function OnAttackCanFullyResolve()
	return obj.IsSameZone() and obj.Exists({q.Location, l.PlayerUnits, q.Other, o.This, q.Other, o.Resting})
end

function OnAttackCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function OnAttack()
	if obj.IsSameZone() then
		obj.Stand({q.Location, l.PlayerRC, q.Other, o.This})
	end
end