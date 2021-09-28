-- Acrobat Presenter

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 0
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.VanguardIs("Diabolos, \"Violence\" Bruce") and (obj.IsAttackingUnit() or obj.IsBooster()) then
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
		obj.SoulCharge(1)
		if obj.InFinalRush() then
			obj.SoulCharge(1)
		end
	end
	return 0
end