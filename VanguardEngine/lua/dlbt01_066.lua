-- Flying Away, Cheluel

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.Attacking, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Selected
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, p.HasPrompt, p.BlackWings, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.Exists(1) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(2) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.SoulCharge(1)
		obj.Select(2)
		obj.AddUntilEndOfNextTurnState(3, cs.Paralyze)
	end
	return 0
end