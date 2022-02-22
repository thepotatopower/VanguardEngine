-- 寄る辺亡き魂よ、 我が見に集え

function RegisterAbilities()
	-- on order
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetGetCosts("GetCosts")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanSpecificDiscard({q.Location, l.Hand, q.Other, o.Order, q.Count, 1}) end
	obj.SpecificDiscard({q.Location, l.Hand, q.Other, o.Order, q.Count, 1})
end

function GetCosts()
	obj.SetParam({q.Location, l.Hand, q.Other, o.Order, q.Count, 1}, 1)
	return p.SpecificDiscard, 1
end

function Activation()
	obj.Select({q.Location, l.PlayerVC, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusDrive, 1, p.UntilEndOfTurn)
end