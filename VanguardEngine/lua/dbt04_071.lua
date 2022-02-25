-- 孤高の霊宝

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanCB(2) end
	obj.CounterBlast(2)
end

function CanFullyResolve()
	return obj.CanAddToHand({q.Location, l.Drop, q.Other, o.Arms, q.Count, 1})
end

function Activation()
	obj.ChooseAddToHand({q.Location, l.Drop, q.Other, o.Arms, q.Count, 1})
end