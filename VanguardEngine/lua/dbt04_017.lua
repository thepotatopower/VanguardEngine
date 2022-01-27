-- 砂塵の双弾 トラヴィス

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetTrigger("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Condition()
	return obj.EnemyRCRetiredThisTurn() >= 1 
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Activation()
	obj.SoulCharge(1)
	obj.ChooseRetire({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1})
	obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 10000, p.UntilEndOfTurn)
end