-- 非業の死を乗り越えて

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetGetCosts("GetCosts")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function GetCosts()
	return p.CB, 1
end

function Activation()
	obj.Bind({q.Other, o.ThisFieldID})
	obj.ChooseAddToHand({q.Location, l.Drop, q.Other, o.Order, q.Count, 2, q.Min, 0})
end
