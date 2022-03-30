-- 立ち向かう勇気、貫く覚悟

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.Deck, q.Grade, 2, q.Other, o.Unit, q.NameContains, obj.LoadName("Blaster")})
end

function Activation()
	obj.Search({q.Location, l.Deck, q.Grade, 2, q.Other, o.Unit, q.NameContains, obj.LoadName("Blaster"), q.Count, 1, q.Min, 0})
end