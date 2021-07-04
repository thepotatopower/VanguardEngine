-- Hollowing Moonlit Night

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, t.Order, p.HasPrompt, true, p.IsMandatory, false
	elseif n == 2 then
		return a.Cont, t.Cont, p.HasPrompt, false, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.CanSB(1) then
			return true
		end
	elseif n == 2 then
		if obj.IsWorld() then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.SoulBlast(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.SetWorld()
		obj.Draw(1)
	elseif n == 2 then
		if obj.OnlyWorlds() then
			if obj.NumWorlds() == 1 then
				obj.DarkNight()
			elseif obj.NumWorlds() >= 2 then
				obj.AbyssalDarkNight()
			end
		else
			obj.NoWorld()
		end
	end
	return 0
end