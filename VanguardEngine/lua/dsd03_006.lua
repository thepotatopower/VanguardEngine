-- 斧鉞の騎士 ラフルク

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanAddToSoul({q.Location, l.PlayerRC, q.Other, o.This}) end
	obj.AddToSoul({q.Location, l.PlayerRC, q.Other, o.This})
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.PlayerRC, q.Grade, 3, q.Other, o.NotThis})
end

function Activation()
	obj.Select({q.Location, l.PlayerRC, q.Grade, 3, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 10000, p.UntilEndOfTurn)
end