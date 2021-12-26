-- 腐肉捌き

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanCB(1) and obj.CanBind({q.Location, l.PlayerRC, q.Other, o.This}) end
	obj.CounterBlast(1)
	obj.Bind({q.Location, l.PlayerRC, q.Other, o.This})
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.Drop, q.Other, o.Order})
end

function Activation()
	obj.ChooseAddToHand({q.Location, l.Drop, q.Other, o.Order, q.Count, 1})
end