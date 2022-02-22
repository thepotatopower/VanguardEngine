-- 罪過に廃絶の美酒を

function RegisterAbilities()
	-- blitz order
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBlitzOrder)
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.Drop, q.Location, l.Bind, q.Other, o.Order})
end

function Activation()
	obj.Draw(1)
	obj.Select({q.Location, l.PlayerUnits, q.Count, 1})
	local count = obj.GetNumberOf({q.Location, l.Drop, q.Location, l.Bind, q.Other, o.Order, q.Other, o.DifferentNames})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 5000 * count, p.UntilEndOfBattle)
end