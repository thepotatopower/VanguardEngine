-- Cardinal Prima, Altepo

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
		return a.PutOnGC, t.Auto, p.HasPrompt, true, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPutOnGC() and (obj.IsDarkNight() or obj.IsAbyssalDarkNight()) then
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
		if obj.IsDarkNight() then
			obj.SoulCharge(1)
		elseif obj.IsAbyssalDarkNight() then
			obj.SoulCharge(2)
		end
	end
	return 0
end