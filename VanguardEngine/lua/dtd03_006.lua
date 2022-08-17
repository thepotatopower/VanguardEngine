-- インターナライズ・メイジ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetLocation(l.RC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetTiming(a.OnACT)
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.setActivation("Activation")
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Youthberk")})
end

function Cost(check)
	if check then return obj.CanCB(2) end
	obj.CounterBlast(2)
end

function Activation()
	obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 10000, p.UntilEndOfTurn)
end