-- 貪りの乱鴉

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Condition()
	return obj.OrderPlayed()
end

function Cost(check)
	if check then return obj.CanSpecificDiscard({q.Location, l.Hand, q.Other, o.Order, q.Count, 1}) end
	obj.SpecificDiscard({q.Location, l.Hand, q.Other, o.Order, q.Count, 1})
end

function Activation()
	obj.Draw(1)
end