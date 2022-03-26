-- パンターレイ・ドラゴン

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
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function Activation()
	obj.Draw(1)
	obj.Discard(1)
	if obj.Exists({q.Location, l.Order}) then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	end
end