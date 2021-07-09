-- Acrobat Presenter

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 0
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, t.Auto, p.HasPrompt, true, p.IsMandatory, true
	elseif n == 2 then
		return a.Then, t.Auto, p.HasPrompt, true, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsAttackingUnit() or obj.IsBooster() then
			return true
		end
	elseif n == 2 then 
		if obj.InFinalRush() then
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
end

function Activate(n)
	if n == 1 then
		obj.SoulCharge(1)
		return 2
	elseif n == 2 then
		obj.SoulCharge(1)
	end
	return 0
end