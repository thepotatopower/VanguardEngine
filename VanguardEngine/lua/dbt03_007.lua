-- 卓絶の天衝 ラグレール

function RegisterAbilities()
	-- on attack
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.PlayerRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit() and obj.Exists({q.Location, l.EnemyVC, q.Other, o.Attacked})
end

function CanFullyResolve()
	return obj.IsSameZone() 
end

function Cost(check)
	if check then return obj.CanCB(2) end
	obj.CounterBlast(2)
	obj.Store(obj.ChooseRest({q.Location, l.PlayerRC, q.Other, o.Standing, q.Min, 0}))
end

function Activation()
	if obj.IsSameZone() then
		obj.AddCardState({q.Location, l.PlayerRC, q.Other, o.This}, cs.CanDriveCheck, p.UntilEndOfBattle)
		obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusPower, 5000, p.UntilEndOfBattle)
	end
	local timedTrigger = NewAbility(GetID())
	timedTrigger.SetTiming(a.OnBattleEnds)
	timedTrigger.SetActivation("TimedTriggerActivation")
	timedTrigger.Store(obj.ValidIDs({q.Location, l.Stored}))
	timedTrigger.SetResetTiming(p.UntilEndOfBattle)
end

function TimedTriggerActivation()
	local count = obj.GetNumberOf({q.Location, l.Stored})
	if count <= 3 then
		obj.Discard(3)
	end
	if count <= 1 then
		obj.Discard(2)
	end
end