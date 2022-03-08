-- アウェイティングスマイル マルエル

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanSB(2) end
	obj.SoulBlast(2)
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.Damage, q.Other, o.FaceDown})
end

function Activation()
	obj.CounterCharge(1)
end