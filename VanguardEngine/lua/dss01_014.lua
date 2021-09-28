-- Diabolos Boys, Jared

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerUnits, q.NameContains, "Diabolos", q.Count, 1
end

function ActivationRequirement(n)
	if n == 1 then
		return a.ACT, p.HasPrompt, p.OncePerTurn, p.CB, 1, p.SB, 1
	elseif n == 2 then
		return a.OnAttack, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() then 
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.IsAttackingUnit() and obj.InFinalRush() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(1) then
			return true
		end
	elseif n == 2 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.ChooseAddTempPower(1, 10000)
	elseif n == 2 then
		obj.EnemyDisableIntercept()
	end
	return 0
end
