-- Star Dragon Deity of Infinitude, Eldobreath

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.FrontRow
	end
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

function Cost(n)
end

function Activate(n)
	if n == 1 then
		obj.DoublePower(1)
		obj.DoubleCritical(1)
	end
	return 0
end