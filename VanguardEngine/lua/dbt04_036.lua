-- 昇華する願い

function RegisterAbilities()
	-- on order
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.PlayerUnits, q.Other, o.OverDress, q.Count, 1})
end

function Activation()
	obj.Select({q.Location, l.PlayerUnits, q.Other, o.OverDress, q.Count, 1})
	if obj.Exists({q.Location, l.Selected}) then
		local timedTrigger = GiveAbility(GetID(), obj.GetID({q.Location, l.Selected}))
		timedTrigger.SetDescription(2)
		timedTrigger.SetTiming(a.OnAttack)
		timedTrigger.SetLocation(l.RC)
		timedTrigger.SetTrigger("OnAttackTrigger")
		timedTrigger.SetCondition("OnAttackCondition")
		timedTrigger.SetActivation("OnAttack")
		timedTrigger.SetResetTarget(obj.GetID({q.Location, l.Selected}))
		timedTrigger.SetResetTiming(p.UntilEndOfTurn)
	end
end

function OnAttackTrigger()
	return obj.IsAttackingUnit() and obj.Exists({q.Location, l.EnemyVC, q.Other, o.Attacked})
end

function OnAttackCondition()
	return obj.CanCounterCharge()
end

function OnAttack()
	obj.CounterCharge(1)
end