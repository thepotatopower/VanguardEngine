-- 砂塵の重砲 ユージン

function RegisterAbilities()
	-- retire
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.VC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetCost("RetireCost")
	ability1.SetActivation("Retire")
	-- call
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnACT)
	ability2.SetLocation(l.VC)
	ability2.SetProperty(p.OncePerTurn)
	ability2.SetCondition("CallCondition")
	ability2.SetCost("CallCost")
	ability2.SetActivation("Call")
end

function RetireCost(check)
	if check then return obj.CanRest({q.Location, l.PlayerRC, q.Other, o.Standing, q.Count, 2}) end
	obj.ChooseRest({q.Location, l.PlayerRC, q.Other, o.Standing, q.Count, 2})
end

function Retire()
	obj.ChooseRetire({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1})
	obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 10000, p.UntilEndOfTurn)
end

function CallCondition()
	return obj.EnemyRetiredThisTurn()
end

function CallCost(check)
	if check then return obj.CanSB(5) end
	obj.SoulBlast(5)
end

function Call()
	obj.LookAtTopOfDeck(obj.NumEnemyOpenCircles())
	obj.SuperiorCall({q.Location, l.Looking, q.Min, 0})
	obj.AddToSoul({q.Location, l.Looking})
end