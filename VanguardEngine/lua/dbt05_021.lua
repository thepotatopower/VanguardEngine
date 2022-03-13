--尖鋭竜 エオラフィアス

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit() or obj.IsBooster()
end

function Cost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function CanFullyResolve()
	return obj.IsSameZone() and obj.Exists({q.Location, l.PlayerRC, q.Grade, 1, q.Other, o.GradeOrLess, q.Other, o.NotThis})
end

function Activation()
	local power = obj.GetNumberOf({q.Location, l.PlayerRC, q.Grade, 1, q.Other, o.GradeOrLess, q.Other, o.NotThis}) * 5000
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, power, p.UntilEndOfBattle)
end