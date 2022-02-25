-- 渦雷を帯びるギアイランド

function RegisterAbilities()
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
	return obj.Exists({q.Location, l.Soul, q.Grade, 3, q.Count, 2})
end

function Cost(check)
	if check then return obj.CanDiscard(1) end
	obj.Discard(1)
end

function Activation()
	obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 15000, p.UntilEndOfTurn)
end