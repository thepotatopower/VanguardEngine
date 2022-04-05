-- ディアブロスマドンナ レジーナ

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
	return obj.InFinalRush()
end

function Cost(check)
	if check then return obj.CanAddToSoul({q.Location, l.PlayerRC, q.Count, 2, q.Other, o.NotThis}) end
	obj.ChooseAddToSoul({q.Location, l.PlayerRC, q.Count, 2, q.Other, o.NotThis})
end

function Activation()
	obj.Store(obj.SuperiorCall({q.Location, l.Soul, q.Count, 1}))
	obj.AddCardValue({q.Location, l.Stored}, cs.BonusPower, 10000, p.UntilEndOfTurn)
	obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 10000, p.UntilEndOfTurn)
end