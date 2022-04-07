-- 呪われし魂は悶え蠢く

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetGetCosts("GetCosts")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanSB(2) end
	obj.SoulBlast(2)
end

function GetCosts()
	return p.SB, 2
end

function Activation()
	obj.LookAtTopOfDeck(4)
	if obj.IsAlchemagic() then
		obj.SuperiorCall({q.Location, l.Looking, q.Other, o.Unit, q.Count, 2, q.Min, 0})
	else
		obj.SuperiorCall({q.Location, l.Looking, q.Other, o.Unit, q.Count, 1, q.Min, 0})
	end
	obj.AddToDrop({q.Location, l.Looking})
	obj.AddCardValue({q.Location, l.LastCalled}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end