-- 転魄の剪竜

function RegisterAbilities()
	-- on end phase
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnEndPhase)
	ability1.SetLocation(l.RC)
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Condition()
	return obj.IsAlchemagic() or obj.NumOfAttacks() >= 5
end

function Cost(check)
	if check then return obj.CanAddToSoul({q.Other, o.ThisFieldID}) end
	obj.AddToSoul({q.Other, o.ThisFieldID})
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.Damage, q.Other, o.FaceDown})
end

function Activation()
	obj.CounterCharge(1)
end