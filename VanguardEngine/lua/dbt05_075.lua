-- 卓識の騎士 アルウィラグス

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanCB(2) end
	obj.CounterBlast(2)
end

function Activation()
	obj.LookAtTopOfDeck(2)
	obj.SuperiorCall({q.Location, l.Looking, q.Other, o.Unit, q.Count, 1, q.Min, 0})
	obj.RearrangeOnTop({q.Location, l.Looking})
end