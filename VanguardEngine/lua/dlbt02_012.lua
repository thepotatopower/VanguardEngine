-- 自分への挑戦 トルディ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnPut)
	ability1.SetMovedFrom(l.Hand, l.Deck)
	ability1.SetMovedTo(l.Drop)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Wilista")})
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.Drop, q.Location, l.Deck, q.Other, o.Gem, q.Count, 1})
end

function Activation()
	obj.Search({q.Location, l.Drop, q.Location, l.Deck, q.Other, o.Gem, q.Count, 1, q.Min, 0})
end