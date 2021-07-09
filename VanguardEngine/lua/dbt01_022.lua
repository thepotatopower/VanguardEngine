-- Light Dragon Diety of Honors, Amartinoa

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
		return a.OnOverTrigger, t.OverTrigger, p.HasPrompt, true, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.OnTriggerZone() then
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
		obj.RearguardDriveCheck()
	end
	return 0
end