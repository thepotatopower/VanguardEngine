-- Source Dragon Deity of Blessings, Blessfavor

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerVC, q.Location, l.PlayerRC, q.Count, 1
	elseif n == 2 then
		return q.Location, l.FrontRow
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOverTrigger, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastRevealedTrigger() then
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
		obj.Draw(1)
		obj.Select({q.Location, l.PlayerUnits, q.Count, 1}, Prompt.Critical)
		obj.AddCardValue({q.Location, l.Selected}, cs.TriggerCritical, 1, p.UntilEndOfTurn)
		obj.AddTempPower(2, 10000)
		obj.Heal()
	end
	return 0
end