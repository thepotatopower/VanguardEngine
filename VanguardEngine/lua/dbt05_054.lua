-- 戦場の歌姫 シュゼット

function RegisterAbilities()
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
	return obj.IsAttackingUnit() and obj.Exists({q.Location, l.EnemyVC, q.Other, o.Attacked})
end

function Cost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function CanFullyResolve()
	return obj.IsSameZone() and obj.Exists({q.Location, l.PlayerUnits, q.Other, o.Resting, q.Other, o.NotThisFieldID})
end

function Activation()
	local power = obj.GetNumberOf({q.Location, l.PlayerUnits, q.Other, o.Resting, q.Other, o.NotThisFieldID}) * 2000
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, power, p.UntilEndOfTurn)
end