-- ベイジングファウンテン テルエス

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetProperty(p.Powerful)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable() and obj.Exists({q.Location, l.EnemyVC, q.Other, o.Attacked})
end

function Condition()
	return obj.SoulCount() == 0
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function CanFullyResolve()
	return obj.IsSameZone() or obj.Exists({q.Location, l.PlayerUnits, q.Other, o.NotThis})
end

function Activation()
	if obj.IsSameZone() then
		local timedTrigger = GiveAbility(GetID(), GetID())
		timedTrigger.SetDescription(2)
		timedTrigger.SetTiming(a.OnAttackHits)
		timedTrigger.SetLocation(l.RC)
		timedTrigger.SetTrigger("TimedTriggerTrigger")
		timedTrigger.SetActivation("TimedTrigger")
		timedTrigger.SetResetTiming(p.UntilEndOfBattle)
		timedTrigger.SetResetTarget(GetID())
	end
	obj.Select({q.Location, l.PlayerUnits, q.Other, o.NotThis, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end

function TimedTriggerTrigger()
	return obj.IsAttackingUnit()
end

function TimedTrigger()
	obj.Draw(1)
end