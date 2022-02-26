-- ディアブロスジェッター ウェイド

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetCost("Cost")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Condition(check)
	return obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Bruce")})
end

function Cost(check)
	if check then return obj.CanAddToSoul({q.Other, o.This}) end
	obj.AddToSoul({q.Other, o.This})
end

function Activation()
	obj.Select({q.Location, l.PlayerVC, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 10000, p.UntilEndOfTurn)
end