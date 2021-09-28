-- Dragonic Overlord

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerVC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.Cont, p.IsMandatory
	elseif n == 2 then
		return a.OnAttackHits, p.HasPrompt, p.OncePerTurn, p.Discard, 1, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() or obj.IsRearguard() then
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() and obj.IsAttackingUnit() then
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

function Activate(n)
	if n == 1 then
		if obj.IsAttackingUnit() and obj.TargetIsEnemyRearguard() then
			obj.EnemyCannotGuardFromHand()
		end
	elseif n == 2 then
		obj.Stand(1)
		obj.AddDrive(1, -1)
	end
	return 0
end