-- Dragonic Overlord

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerHand, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerVC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.Cont, t.Cont, p.HasPrompt, false, p.IsMandatory, true
	elseif n == 2 then
		return a.OnAttackHits, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.Discard, 1, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() or obj.IsRearguard() then
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() and not obj.Activated() and obj.IsAttackingUnit() and obj.CanCB(1) and obj.CanDiscard(2) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	elseif n == 2 then
		return true
	end
	return false
end

function Cost(n)
	if n == 2 then
		obj.CounterBlast(1)
		obj.Discard(2)
	end
end

function Activate(n)
	if n == 1 then
		if obj.IsAttackingUnit() and obj.TargetIsEnemyRearguard() then
			obj.EnemyCannotGuardFromHand()
		end
	elseif n == 2 then
		obj.Stand(3)
		obj.AddDrive(3, -1)
	end
	return 0
end