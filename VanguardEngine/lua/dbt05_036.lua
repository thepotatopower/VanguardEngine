-- プラドパラン ラフィロス

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
	if check then return obj.CanCB(1) and obj.CanRetire({q.Location, l.PlayerRC, q.Other, o.NotThis, q.Count, 2}) end
	obj.CounterBlast(1)
	obj.ChooseRetire({q.Location, l.PlayerRC, q.Other, o.NotThis, q.Count, 2})
end

function Activation()
	obj.Draw(1)
	obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end