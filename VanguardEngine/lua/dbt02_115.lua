-- Sylvan Horned Beast, Tealuf (this card will be addressed later)

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 0
end

function GetParam(n)
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttackHits, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsBooster() and obj.EnemyVanguardHit() then
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

function Cost(n)
end

function Activate(n)
	if n == 1 then
		obj.Mill(5)
	end
	return 0
end