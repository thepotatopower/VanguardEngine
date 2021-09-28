-- Alert Guard Gunner

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.EnemyRC, q.Count, 2, q.Min, 0
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttackHitsVanguard, p.HasPrompt
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsAttackingUnit() and obj.HasPrison() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.ChooseImprison(1)
	end
	return 0
end