-- プライズド・トライデント

function RegisterAbilities()
	-- on attack
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.Exists({q.Location, l.PlayerUnits, q.Other, o.Attacking, q.Other, o.NotThis}) 
end

function Condition()
	return obj.NumOfAttacks() >= 5 
end

function Cost(check)
	if check then return obj.CanAddToSoul({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}) end
	obj.AddToSoul({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID})
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.Damage, q.Other, o.FaceDown})
end

function Activation()
	obj.CounterCharge(1)
end