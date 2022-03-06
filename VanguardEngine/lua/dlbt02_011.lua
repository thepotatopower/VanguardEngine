-- Sweet×Sweet

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Activation()
	obj.LookAtTopOfDeck(5)
	obj.SuperiorCall({q.Location, l.Looking, q.Other, o.Unit, q.Count, 1, q.Min, 0})
	obj.Shuffle()
end