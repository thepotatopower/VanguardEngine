-- Acrobat Presenter

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 0
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, false, true
	elseif n == 2 then
		return a.Then, false, true
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