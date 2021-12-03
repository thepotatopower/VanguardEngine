-- 網羅の天盾 フェリシーダ

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Condition()
	return obj.PersonaRode()
end

function Cost(check)
	if check then return obj.CanCB(2) and obj.CanRetire({q.Location, l.PlayerRC, q.Other, o.This}) end
	obj.CounterBlast(2)
	obj.Retire({q.Location, l.PlayerRC, q.Other, o.This})
end

function Activation()
	obj.Select({q.Location, l.PlayerUnits, q.Grade, 3, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusDrive, 1, p.UntilEndOfTurn)
end