-- 哨戒竜 スカウトプテラ

function RegisterAbilities()
	-- on attack
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsBoosted()
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.EnemyRC, q.Grade, 2, q.Other, o.CanChoose, q.Other, o.GradeOrLess}) or obj.Exists({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID})
end

function Activation()
	obj.ChooseRetire({q.Location, l.EnemyRC, q.Grade, 2, q.Other, o.CanChoose, q.Other, o.GradeOrLess, q.Count, 1})
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}, cs.BonusPower, 2000, p.UntilEndOfBattle)
end