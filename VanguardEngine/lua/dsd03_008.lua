-- プラチナム・ウルフ

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanSB(2) end
	obj.SoulBlast(2)
end

function Activation()
	obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end